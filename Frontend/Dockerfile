FROM node:23-alpine as builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run check
RUN npm run build
RUN npm prune --production

FROM node:22-alpine
ARG API_URL
ENV PUBLIC_API_URL $API_URL
WORKDIR /app
COPY --from=builder /app/build build/
COPY --from=builder /app/node_modules node_modules/
COPY package.json .
EXPOSE 3000
ENV NODE_ENV=production
CMD [ "node", "build" ]