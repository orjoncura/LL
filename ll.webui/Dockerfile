# Step 1: Build the application
FROM node:18-alpine AS builder

WORKDIR /app

ARG NEXT_PUBLIC_Version
ENV NEXT_PUBLIC_Version=$NEXT_PUBLIC_Version
ENV NEXT_PUBLIC_API_URL=https://api.fluente.dynv6.net

# Install dependencies first (using a separate copy to take advantage of Docker cache)
COPY package.json package-lock.json* yarn.lock* ./
RUN yarn install --frozen-lockfile || npm install

# Copy the rest of your application code
COPY . .

# Build the Next.js app
RUN yarn build || npm run build

# Step 2: Run the application using a minimal image
FROM node:18-alpine AS runner

# Set working directory
WORKDIR /app

# Only copy the necessary files from builder
COPY --from=builder /app/public ./public
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json

# Set environment variable to tell Next.js it's running in development
ENV NODE_ENV=development

# Expose the port the app runs on
EXPOSE 3000

# Start the app
CMD ["node_modules/.bin/next", "start"]

