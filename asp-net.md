# Deploying ASP.NET app to Virtual Machine

1. Create Windows Server VM Using azure portal
2. Connect VM with RDP
3. Open Powershell and install the pre-requisites

    ```
    Install-WindowsFeature -name Web-Server -IncludeManagementTools

    # Install ASP.NET 4.6
    Install-WindowsFeature Web-Asp-Net45

    # Install Web Management Service
    Install-WindowsFeature -Name Web-Mgmt-Service
    ```