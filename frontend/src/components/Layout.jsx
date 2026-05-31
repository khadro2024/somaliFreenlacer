import { Outlet } from 'react-router-dom';
import Navbar from './Navbar';

export default function Layout() {
  return (
    <div className="flex min-h-screen flex-col">
      <Navbar />
      <main className="flex-1 px-4 py-8 sm:px-6 lg:px-8">
        <Outlet />
      </main>
      <footer className="border-t border-sfm-border/60 bg-sfm-surface/50 py-8">
        <div className="mx-auto max-w-7xl px-4 text-center text-sm text-gray-500 sm:px-6">
          <p>&copy; 2026 Somali Freelance Marketplace (SFM). Dhammaan xuquuqda way xafidan yihiin.</p>
        </div>
      </footer>
    </div>
  );
}
