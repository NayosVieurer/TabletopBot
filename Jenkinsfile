pipeline {
    agent any

    stages {
        stage('Restauration & Build') {
            steps {
                echo 'Téléchargement des paquets et compilation...'
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Tests Unitaires') {
            steps {
                echo 'Exécution des tests de la branche...'
                sh 'dotnet test --configuration Release --no-restore --no-build'
            }
        }

        stage('Déploiement en Production') {
            // SÉCURITÉ : Jenkins n'exécute cette étape QUE si le code est sur la branche 'main'
            when {
                branch 'main'
            }
            steps {
                echo 'Branche main validée. Déploiement en cours sur le Kimsufi...'
                sh 'dotnet publish --configuration Release --output ./publish --no-build'

                sh 'cp -R ./publish/* /var/www/tabletobclubbot/'

                sh 'sudo systemctl restart tabletobclubbot.service'
            }
        }
    }
}
