# Container Registry

1.  Container Image name 

    Docker Uses "Image Id" to identify an image.
    A Single image can actually have multiple names.

2.  A Fully Qualified Container image name contains:

    [registry-url]/[repository]:[TAG]

    Where:
    registry-url:   Default= docker.io, OnPremise= 192.167.12.23 or registry.mycompany.com
                    Microsoft= mcr.microsoft.com/
    
    repository:     Collection of Images for a single application,
                    Multiple versions of single application.

                    library/mysql, mahendrshinde/myweb
    
    TAG:            Individual Version of Image

    Example:
    mahendrshinde/myweb:2

    Where:
    Registry-URL is MISSING, defaults to `docker.io`
    Resulting Image name:
    docker.io/mahendrshinde/myweb:2

    Repository Name is `mahendrshinde/myweb` 
    NOTE: `mahendrshinde` is docker-id

    TAG is `2`

> Registry is SERVICE that ALLOWS creating, accessing REPOSITORIES
> Repository is JUST COLLECTION of images