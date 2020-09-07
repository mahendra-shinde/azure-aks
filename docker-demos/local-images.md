# Save / Load local images

> Backup of Local images as TAR file (tar is Archival without compression)

$ docker save -o [PATH-TO-TARfile] [IMAGE-NAME1]  ... [IMAGE-NAME-n]

> Restore the backed image from TAR file

$ docker load -i [PATH-TO-TARfile]