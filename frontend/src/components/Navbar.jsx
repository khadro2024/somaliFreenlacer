import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Button from './ui/Button';

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const dashboardPath = () => {
    if (!user) return '/login';
    if (user.role === 'Admin') return '/admin';
    if (user.role === 'Client') return '/client';
    return '/freelancer';
  };

  const navLink =
    'text-sm font-medium text-gray-400 transition hover:text-sfm-green-400';

  return (
    <header className="sticky top-0 z-50 border-b border-sfm-border/60 bg-sfm-black/80 backdrop-blur-xl">
      <div className="mx-auto flex max-w-7xl items-center justify-between px-4 py-4 sm:px-6 lg:px-8">
        <Link to="/" className="flex items-center gap-3">
          <span className="flex h-10 w-10 items-center justify-center rounded-xl bg-gradient-to-br from-sfm-green-700 to-sfm-green-900 text-sm font-bold text-sfm-green-300 shadow-lg shadow-sfm-green-900/40">
            SFM
          </span>
          <span className="hidden text-lg font-bold text-white sm:block">
            Somali <span className="text-sfm-green-400">Freelance</span>
          </span>
        </Link>

        <nav className="flex items-center gap-1 sm:gap-6">
          <Link to="/jobs" className={`${navLink} px-3 py-2`}>Jobs</Link>
          <Link to="/freelancers" className={`${navLink} hidden sm:block px-3 py-2`}>Freelancers</Link>
          <Link to="/about" className={`${navLink} hidden md:block px-3 py-2`}>About</Link>
          {user ? (
            <>
              <Link
                to={dashboardPath()}
                className="rounded-lg bg-sfm-green-950/60 px-3 py-2 text-sm font-semibold text-sfm-green-400 ring-1 ring-sfm-green-800/50 transition hover:bg-sfm-green-900/40"
              >
                Dashboard
              </Link>
              <Button variant="outline" size="sm" onClick={() => { logout(); navigate('/'); }}>
                Logout
              </Button>
            </>
          ) : (
            <>
              <Link to="/login" className={`${navLink} px-3 py-2`}>Login</Link>
              <Link to="/register" className="inline-flex items-center justify-center rounded-xl bg-gradient-to-r from-sfm-green-800 to-sfm-green-900 px-4 py-2 text-sm font-semibold text-white shadow-lg shadow-sfm-green-900/30 transition hover:from-sfm-green-700">
                Register
              </Link>
            </>
          )}
        </nav>
      </div>
    </header>
  );
}
