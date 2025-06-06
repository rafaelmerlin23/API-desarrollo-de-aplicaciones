pipeline {
    agent none

    environment {
        IMAGE_NAME   = 'punch_finisher'
        REGISTRY_URL = 'https://index.docker.io/v1/'
    }

    stages {
        stage('Checkout & Build .NET') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args  '-v /var/run/docker.sock:/var/run/docker.sock'
                }
            }
            steps {
                checkout scm
                sh 'dotnet publish -c Release -o out'
            }
        }

        stage('Build Docker image') {
            agent any
            steps {
                script {
                    env.COMMIT = sh(
                        script: 'git rev-parse --short HEAD',
                        returnStdout: true
                    ).trim()
                    docker.build("${IMAGE_NAME}:${env.COMMIT}")
                }
            }
        }

        stage('Push to every collaborator') {
            agent any
            steps {
                script {
                    def collaborators = [
                        [user: 'maizenauwu', creds: 'dockerhub-rafa'],
                        [user: 'cheese400',  creds: 'dockerhub-alex'],
                        [user: 'luisdiaz7',  creds: 'dockerhub-luis'],
                        [user: 'barlau45',  creds: 'dockerhub-jorge']
                    ]

                    def parallelPushes = collaborators.collectEntries { c ->
                        ["push-${c.user}": {
                            docker.withRegistry(REGISTRY_URL, c.creds) {
                                def remoteTag = "${c.user}/${IMAGE_NAME}:${env.COMMIT}"
                                sh "docker tag ${IMAGE_NAME}:${env.COMMIT} ${remoteTag}"
                                sh "docker push ${remoteTag}"
                            }
                        }]
                    }

                    parallel parallelPushes
                }
            }
        }

        /*  ──► Limpieza de workspace  ◄──
            Al ejecutarse en su propio agente, cleanWs()
            ya tiene un contexto de nodo válido (sin label). */
        stage('Cleanup workspace') {
            agent any
            steps {
                cleanWs()
            }
        }
    }
}

