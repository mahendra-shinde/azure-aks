## Dockerfile is RECOMMANDED approach for building container images

1. Create a file 'Dockerfile' (Filename MUST EXACTLY MATCH)
2. Use docker-cli command 'docker build' to actually build image

    > The "DOT" at the end of command specifies CURRENT WORKING DIRECTORY\
    > It also means location of Dockerfile and other project files

    $ docker build -t mywebapp   .

3.  Test run the container using new image 'mywebapp'

    $ docker run -d --name c2 -p 8080:80 mywebapp
    $ start http://localhost:8080
    $ docker stop c2
    $ docker rm c2
    
