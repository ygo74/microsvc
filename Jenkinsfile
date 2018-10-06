pipeline {
  agent any
  stages {
    stage('checkout') {
      steps {
        git(changelog: true, poll: true, url: 'https://github.com/ygo74/microsvc.git', branch: 'master')
      }
    }
    stage('test') {
      steps {
        readFile 'README.MD'
      }
    }
  }
}