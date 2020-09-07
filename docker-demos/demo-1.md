# Docker Container Demo (NGINX)

1.  Download the container image (Search nginx in 'https://hub.docker.com')

    $ docker pull nginx

2.  Launch a new container with name 'c1' in DETACHED mode and expose the port '80' from nginx to local port '8080'

    $ docker run --name c1 -d -p 8080:80 nginx


3.  View the Container status, logs & processes running inside container

    $ docker ps 
    $ docker logs c1
    $ docker top c1

4.  View the output on web-browser using URL: `http://localhost:8080`

5.  For DEBUGGING, you can enter inside a running container (SH is Linux Shell just like CMD for windows).

    $ docker exec -it c1 sh
    $ cd /usr/share/nginx/html
    $ rm index.html
    $ echo "<h1>Hello World from earth</h1>" > index.html
    $ exit

6.  View the output on web-browser using URL: `http://localhost:8080`

7.  Capture and Convert container 'c1' into a new IMAGE.

    $ docker stop c1
    $ docker commit c1 myhelloapp:1
    $ docker images myhelloapp

8.  Remove the OLD 'c1' container and create a new 'c1' from the new image 'myhelloapp:1"

    $ docker rm c1
    $ docker run --name c1 -d -p 8080:80 myhelloapp:1

9.  View the output on web-browser using URL: `http://localhost:8080`
