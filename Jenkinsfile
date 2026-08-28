pipeline {
    agent any

    environment {
        // Isolation des paquets .NET sur le serveur Jenkins
        DOTNET_CLI_HOME = "/tmp/dotnet_home"
    }

    stages {
        stage('Restauration & Build') {
            steps {
                // Notifie GitHub que Jenkins commence à travailler pour bloquer la PR
                githubNotify(status: 'PENDING', context: 'continuous-integration/jenkins', description: 'Restauration et compilation .NET 8...')
                
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

                // Remplacement du # par // pour éviter l'erreur de syntaxe Groovy
                // Copie les fichiers vers le dossier de prod
                sh 'cp -R ./publish/* /var/www/tabletobclubbot/'

                // Redémarre le service grâce aux droits sudo configurés précédemment
                sh 'sudo systemctl restart tabletobclubbot.service'
            }
        }
    }

    post {
        success {
            // Débloque le bouton de fusion (Merge) sur GitHub
            githubNotify(status: 'SUCCESS', context: 'continuous-integration/jenkins', description: 'Le build et les tests sont validés ! ✅')
        }
        failure {
            // Laisse le bouton de fusion bloqué si le build ou les tests échouent
            githubNotify(status: 'FAILURE', context: 'continuous-integration/jenkins', description: 'Échec de la compilation ou des tests. ❌')
        }
    }
}
