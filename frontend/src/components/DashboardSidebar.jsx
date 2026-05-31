import { NavLink } from 'react-router-dom';

const linkClass = ({ isActive }) =>
  `block rounded-xl px-4 py-2.5 text-sm font-medium transition ${
    isActive
      ? 'bg-sfm-green-900/50 text-sfm-green-300 ring-1 ring-sfm-green-700/50'
      : 'text-gray-400 hover:bg-white/5 hover:text-sfm-green-400'
  }`;

export default function DashboardSidebar({ title, links }) {
  return (
    <aside className="h-fit rounded-2xl border border-sfm-border/80 bg-sfm-card/60 p-4 backdrop-blur-sm lg:sticky lg:top-24">
      <p className="mb-4 px-2 text-xs font-bold uppercase tracking-widest text-sfm-green-500">{title}</p>
      <nav className="flex flex-col gap-1">
        {links.map((link) => (
          <NavLink key={link.to} to={link.to} end={link.end} className={linkClass}>
            {link.label}
          </NavLink>
        ))}
      </nav>
    </aside>
  );
}
