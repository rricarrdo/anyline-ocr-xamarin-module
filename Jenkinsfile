pipeline {
  agent {
    node {
      label 'master'
    }

  }
  stages {
    stage('Build') {
      steps {
        build 'Xamarin Android SDK'
      }
    }

    stage('Print') {
      steps {
        echo 'Done'
      }
    }

  }
}