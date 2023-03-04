FROM python:3
COPY SavePlayerPosition.py /app/

COPY requirements.txt /app/
WORKDIR /app
RUN pip install -r requirements.txt

CMD ["python", "SavePlayerPosition.py"]


