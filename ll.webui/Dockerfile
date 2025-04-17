FROM node:14
ENV NODE_ENV=production
WORKDIR /src
COPY . .
RUN npm install
COPY . .
ENTRYPOINT ["node", "src/app/page.tsx"]
EXPOSE 3000
