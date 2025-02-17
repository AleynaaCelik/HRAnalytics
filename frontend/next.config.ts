import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  async rewrites() {
    return [
      {
        source: '/api/:path*',
        destination: 'https://localhost:7264/api/:path*' // API'nizin çalıştığı port
      }
    ]
  }
};

export default nextConfig;