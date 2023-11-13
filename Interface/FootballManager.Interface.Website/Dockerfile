FROM nginx:1.25.1-alpine
# COPY ./default.conf /etc/nginx/conf.d/default.conf
COPY /build /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]