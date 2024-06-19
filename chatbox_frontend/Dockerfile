# pull official base image
FROM node:22-alpine3.19

# set working directory
WORKDIR /app

# install app dependencies
COPY package*.json ./
RUN npm install

COPY . .

# start app
CMD npm run dev