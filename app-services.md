#Azure App Service

1. App Service Plan is an object that reserves instances (PaaS VM) for All 'Apps'
2. App Service Plans
    - Free Plan (Shared Infrastructure) F1
    - Shared Plan (Shared Infrastructure) D1
    - Dedicated Plans B1,2,3;  S1,2,3;  P1,2,3;
    - Isolated Plan (App Service Environments) Reserved Instances for PaaS
3. App Service [OS Types]
    - Windows
    - Linux
    - Single Resource group CANNOT have both Linux & Windows apps
4. App Service [Deployment Types]
    - Code [Classic] Use 'WebDeploy' or 'FTP' 
    - Container [Single Container or Multi-Container]

5. Deployment Slots (Multiple CLONEs of Application)
    Number of "Slots" depends on "App Service Plan"

 