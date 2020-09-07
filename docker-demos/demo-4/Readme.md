## ASP.NET Core Web Application in Containers

1.  Create new or Copy existing asp.net application name "mywebapp"

    $ dotnet new mvc -n mywebapp

2.  Build and Publish application to subfolder "app"

    $ cd mywebapp
    $ dotnet publish -o app/

3.  Verify the binaries created by "publish"

    $ cd app
    $ dir
    $ cd ../..

4.  Create a new Dockerfile 

    ```
    FROM mcr.microsoft.com/dotnet/core/aspnet

    WORKDIR /app

    COPY mywebapp/app/ /app

    CMD ["dotnet","/app/mywebapp.dll"]
    ```

5.  Now, Use following commands, to build and test run the image

    ```
    ## Check if you are in correct location
    $ dir 
    ## expected "Dockerfile" and "mywebapp" folder
    $ docker build -t myaspapp .
    ### Use following command in powershell to delete all running container 
    $ docker rm $(docker ps -aq)
    ## Create a test container and then delete
    $ docker run --name c5 -d -p 8080:80 myaspapp
    $ start http://localhost:8080
    $ docker rm -f c5
    ```

6.  Multi Stage Build
    
    > Keep Smaller `final image` size by using different `stages`
    > Allows Build before packaging to verify app before containerization.

7.  Rename the old `Dockerfile` AS `Dockerfile.old` and create a new `Dockerfile` as below:

    ```
    FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as buildStage
    WORKDIR /app
    COPY mywebapp/ /app
    RUN ["dotnet","restore"]
    RUN ["dotnet","publish","-o","dist"]

    ## final stage
    FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
    COPY --from=buildStage /app/dist/ /app
    CMD ["dotnet","/app/mywebapp.dll"]
    ```

8.  Build the image and TEST it!

    ```
    $ docker build -t myaspapp:2 .
    $ docker rm $(docker ps -aq)
    $ docker run --name c5 -d -p 8080:80 myaspapp:2
    $ start http://localhost:8080/
    $ docker rm -f c5
    ```
