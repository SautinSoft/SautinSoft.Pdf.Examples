# Execute docker commands from project's folder

```cmd
:: 1. Create docker image named "pdfimage".
docker build -t pdfimage -f Dockerfile .

:: 2. Create and start docker container named "pdfcontainer".
docker run --name pdfcontainer pdfimage

:: 3. Copy output files from container to project's folder.
docker cp pdfcontainer:/app/test.pdf .
docker cp pdfcontainer:/app/test.docx .

:: 4. Clean up, remove container and image.
docker container rm pdfcontainer
docker image rm pdfimage
```