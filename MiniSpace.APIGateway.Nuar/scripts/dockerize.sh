#!/bin/bash
TAG=''
VERSION_TAG=

case "$TRAVIS_BRANCH" in
  "master")
    TAG=latest
    VERSION_TAG=$TRAVIS_BUILD_NUMBER
    ;;
  "develop")
    TAG=dev
    VERSION_TAG=$TAG-$TRAVIS_BUILD_NUMBER
    ;;    
esac

REPOSITORY=$DOCKER_USERNAME/minispace.apigateway

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -t $REPOSITORY:$TAG -t $REPOSITORY:$VERSION_TAG .
docker push $REPOSITORY:$TAG
docker push $REPOSITORY:$VERSION_TAG