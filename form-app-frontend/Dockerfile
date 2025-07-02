# Build stage
FROM node:18-alpine as build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
# Fix the build command - newer Angular versions don't use --prod flag
RUN npm run build

# Production stage
FROM nginx:alpine
# Update this path - newer Angular builds might have a different structure
COPY --from=build /app/dist/form-app-frontend/browser /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]