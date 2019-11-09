stage("Che  ckout") {
    try {
      node('master') {
            checkout scm
      }
    } catch (err){
      emailext to:"dmitry.tkachenko@talkremit.com", subject:"ERROR in ${JOB_NAME} #${BUILD_NUMBER}! ", body: "Check console output at ${BUILD_URL} to view the results."
      throw err
    }
}

stage("Build IMG") {
    try {
        node('master') {
            GIT_COMMIT_HASH=sh(returnStdout: true, script: "git log -n 1 --pretty=format:'%h'").trim()

            sh """
                echo $GIT_COMMIT_HASH
                docker build --build-arg WATERMARK=${GIT_COMMIT_HASH} -t empty_microservice --no-cache .
            """
}
    } catch (err){
      emailext to:"dmitry.tkachenko@talkremit.com", subject:"ERROR in ${JOB_NAME} #${BUILD_NUMBER}! ", body: "Check console output at ${BUILD_URL} to view the results."
      throw err
    }
}

stage("Push IMG"){
  try {
    node('master') {
      GIT_COMMIT_HASH=sh(returnStdout: true, script: "git log -n 1 --pretty=format:'%h'").trim()

      docker.withRegistry("https://639061735319.dkr.ecr.eu-west-1.amazonaws.com/empty_microservice", "ecr:eu-west-1:ECR") {
        docker.image("empty_microservice").push("latest")
        docker.image("empty_microservice").push("${GIT_COMMIT_HASH}")
      }
    }
  } catch (err){
      emailext to:"dmitry.tkachenko@talkremit.com", subject:"ERROR in ${JOB_NAME} #${BUILD_NUMBER}! ", body: "Check console output at ${BUILD_URL} to view the results."
    throw err
  }
}

stage("Clear IMG"){
  try {
    node('master') {
      sh "docker image prune -a -f"
    }
  } catch (err){
      emailext to:"dmitry.tkachenko@talkremit.com", subject:"ERROR in ${JOB_NAME} #${BUILD_NUMBER}! ", body: "Check console output at ${BUILD_URL} to view the results."
    throw err
  }
}
