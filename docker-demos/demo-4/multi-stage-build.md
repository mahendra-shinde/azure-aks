## Multi Stage Dockerfile

### Section
FROM [BaseImage1] as stage1

COPY ...
RUN ...

### Section
FROM [BaseImage2] as stage2

COPY --from=stage1 ...
RUN ...

### Section
FROM [BaseImage3] 

COPY ...
RUN ...
CMD ...
