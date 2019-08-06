FROM node:lts-alpine as build-stage
COPY /wwwroot /app
RUN ls /app

FROM nginx:stable-alpine as production-stage
COPY default.conf /etc/nginx/conf.d/default.conf
RUN ls /etc/nginx/conf.d/
RUN cat /etc/nginx/conf.d/default.conf
COPY --from=build-stage /app /usr/share/nginx/html
RUN ls /usr/share/nginx/html
EXPOSE 8007
CMD ["nginx", "-g", "daemon off;"]

LABEL maintainer "agentschap Informatie Vlaanderen <informatie.vlaanderen@vlaanderen.be>"
