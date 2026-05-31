export default function StatCard({ value, label, icon }) {
  return (
    <div className="group relative overflow-hidden rounded-2xl border border-sfm-green-900/40 bg-gradient-to-br from-sfm-card to-sfm-green-950/30 p-6 text-center transition hover:border-sfm-green-700/50">
      <div className="absolute -right-4 -top-4 h-24 w-24 rounded-full bg-sfm-green-500/5 blur-2xl transition group-hover:bg-sfm-green-500/10" />
      {icon && <div className="mb-2 text-2xl">{icon}</div>}
      <p className="text-3xl font-bold text-sfm-green-400">{value}</p>
      <p className="mt-1 text-sm text-gray-500">{label}</p>
    </div>
  );
}
