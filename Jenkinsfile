pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = "/tmp/dotnet_home"
    }

    stages {
        stage('Restauration & Build') {
            steps {
                // Syntaxe exacte pour le GitHub Integration Plugin
                gitHubStatusNotify(status: 'PENDING', context: 'continuous-integration/jenkins', description: 'Restauration et compilation .NET 8...')
                
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

    post {
        success {
            gitHubStatusNotify(status: 'SUCCESS', context: 'continuous-integration/jenkins', description: 'Le build et les tests sont validés ! ✅')
        }
        failure {
            gitHubStatusNotify(status: 'FAILURE', context: 'continuous-integration/jenkins', description: 'Échec de la compilation ou des tests. ❌')
        }
    }
}
