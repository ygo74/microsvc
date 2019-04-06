pipeline {
  agent any
  stages {
    stage('checkout') {
      steps {
        git(changelog: true, poll: true, url: 'https://github.com/ygo74/microsvc.git', branch: 'master')
        ansiblePlaybook(playbook: 'test.yml', become: true)
      }
    }
    stage('test') {
      steps {
        readFile 'README.md'
      }
    }
  }
}