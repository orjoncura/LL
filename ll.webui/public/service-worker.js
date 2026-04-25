const CACHE_NAME = 'll-app-shell-v1';
const APP_SHELL = ['/', '/manifest.json', '/icon.png', '/badge.png'];

self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => cache.addAll(APP_SHELL)).then(() => self.skipWaiting())
  );
});

self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then((keys) =>
      Promise.all(keys.filter((key) => key !== CACHE_NAME).map((key) => caches.delete(key)))
    ).then(() => self.clients.claim())
  );
});

self.addEventListener('fetch', (event) => {
  if (event.request.method !== 'GET') return;

  const isHttp = event.request.url.startsWith('http');
  if (!isHttp) return;

  event.respondWith(
    caches.match(event.request).then((cached) => {
      if (cached) return cached;

      return fetch(event.request)
        .then((response) => {
          if (!response || response.status !== 200 || response.type !== 'basic') {
            return response;
          }

          const responseClone = response.clone();
          caches.open(CACHE_NAME).then((cache) => cache.put(event.request, responseClone));
          return response;
        })
        .catch(() => caches.match('/'));
    })
  );
});

self.addEventListener('push', (event) => {
  event.waitUntil(
    (async () => {
      let data = {
        title: 'Default Title',
        body: 'Default Body',
        icon: '/icon.png',
        badge: '/badge.png',
      };

      try {
        if (event.data) {
          const json = event.data.json();
          data.title = json.title || data.title;
          data.body = json.body || data.body;
          data.icon = json.icon || data.icon;
          data.badge = json.badge || data.badge;
        }
      } catch (e) {
        data.body = await event.data.text(); // fallback for plain text
      }

      return self.registration.showNotification(data.title, {
        body: data.body,
        icon: data.icon,
        badge: data.badge,
        data: {
          url: data.url || '/',
        },
      });
    })()
  );
});

self.addEventListener('notificationclick', (event) => {
  event.notification.close();
  const targetUrl = event.notification.data || '/';

  event.waitUntil(
    clients.matchAll({ type: 'window', includeUncontrolled: true }).then((clientList) => {
      for (const client of clientList) {
        if (client.url === targetUrl && 'focus' in client) {
          return client.focus();
        }
      }
      if (clients.openWindow) {
        return clients.openWindow(targetUrl);
      }
    })
  );
});
