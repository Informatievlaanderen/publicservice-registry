#!/bin/sh

CONTAINERID=$(curl -s http://169.254.170.2/v2/metadata | jq -r ".Containers[] | select(.Labels[\"com.amazonaws.ecs.container-name\"] | contains(\"basisregisters-\") and contains(\"-ui\")) | .DockerId")

echo "window.dienstverleningVersion=\"v2-$API_VERSION\";" >> /usr/share/nginx/html/config.js
echo "window.dienstverleningApiEndpoint=\"$API_ENDPOINT\";" >> /usr/share/nginx/html/config.js

nginx -g daemon off
