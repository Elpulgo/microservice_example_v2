# build
FROM node:alpine as build
WORKDIR /app
COPY ./Client/package.json .
RUN npm install
COPY ./Client .
RUN npm run build --prod

# deploy
FROM nginx:1.19.3-alpine
EXPOSE 80
COPY ./Client/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/Client /usr/share/nginx/html