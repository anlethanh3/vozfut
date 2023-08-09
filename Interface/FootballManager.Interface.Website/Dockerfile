FROM nginx:1.25.1-alpine as production-stage
COPY ./default.conf /etc/nginx/conf.d/default.conf
COPY /build /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]