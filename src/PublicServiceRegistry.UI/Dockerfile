FROM nginx:stable-alpine as production-stage

LABEL maintainer "agentschap Informatie Vlaanderen <informatie.vlaanderen@vlaanderen.be>"

COPY default.conf /etc/nginx/conf.d/default.conf
COPY /wwwroot /usr/share/nginx/html
COPY config.js /usr/share/nginx/html/config.js
COPY init.sh ./init.sh

RUN chmod +x ./init.sh

EXPOSE 8007

ENTRYPOINT ["./init.sh"]
