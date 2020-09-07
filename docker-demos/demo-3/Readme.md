## Containerized Angular App 

1.  Created a new Angular project / Copy existing project into current directory

    New Project: ng new --minimal app1

2.  Build the angular project and create "dist" for deployment

    $ ng build 

3.  Create a Dockerfile in current directory (Outside "app1")

    ```
    FROM nginx

    COPY app1/dist/app1 /usr/share/nginx/html
    ```

4.  Build a new image and create test container

    ```
    $ docker build -t myngapp . 
    $ docker run --name c4 -d -p 8080:80 myngapp
    $ start http://localhost:8080
    $ docker stop c4
    $ docker rm c4
    ```
