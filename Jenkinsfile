pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = "/tmp/dotnet_home"
    }

    stages {
        stage('Restauration & Build') {
            steps {
                // Notifie GitHub universellement du début du build
                step([$class: 'GitHubCommitStatusSetter', 
                      contextSource: [$class: 'ManuallyEnteredCommitStatusContextSource', context: 'continuous-integration/jenkins'],
                      statusResultSource: [$class: 'ConditionalStatusResultSource', results: [[$class: 'AnyBuildResult', state: 'PENDING', message: 'Restauration et compilation .NET 8...']]]
                ])
                
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

    post {
        success {
            // Débloque le bouton de fusion (Merge) sur GitHub
            step([$class: 'GitHubCommitStatusSetter', 
                  contextSource: [$class: 'ManuallyEnteredCommitStatusContextSource', context: 'continuous-integration/jenkins'],
                  statusResultSource: [$class: 'ConditionalStatusResultSource', results: [[$class: 'AnyBuildResult', state: 'SUCCESS', message: 'Le build et les tests sont validés ! ✅']]]
            ])
        }
        failure {
            // Laisse le bouton de fusion bloqué si le build ou les tests échouent
            step([$class: 'GitHubCommitStatusSetter', 
                  contextSource: [$class: 'ManuallyEnteredCommitStatusContextSource', context: 'continuous-integration/jenkins'],
                  statusResultSource: [$class: 'ConditionalStatusResultSource', results: [[$class: 'AnyBuildResult', state: 'FAILURE', message: 'Échec de la compilation ou des tests. ❌']]]
            ])
        }
    }
}
