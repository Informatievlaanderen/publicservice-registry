#!/bin/sh

echo "window.dienstverleningVersion=\"v2-$API_VERSION\";" >> /usr/share/nginx/html/config.js
echo "window.dienstverleningApiEndpoint=\"$API_ENDPOINT\";" >> /usr/share/nginx/html/config.js

nginx -g 'daemon off;'
