pipeline {
  agent {
    node {
      label 'windows'
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