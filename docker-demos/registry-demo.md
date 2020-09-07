# Registry Demo

1.  Login in azure portal https://portal.azure.com

2.  Create a new `Azure Container Registry` with name `myreg00001` [Must be UNIQUE]

3.  Goto Access Keys and Enable Admin User, copy following details for later use:

    - Admin Username
    - Login Server URL
    - Password

4.  Open Powershell or Command prompt and login using docker-cli (All values from previous step without square braces)

    ```
    $ docker login [LoginServerURL]
    Enter Username: [Admin Username]
    Enter Password: [Password]
    ```

5.  Pick one image that you need to push to registry 

    ```
    $ docker tag myaspapp:3 [LoginServerURL]/myaspapp:3
    $ docker push [LoginServerURL]/myaspapp:3
    ```