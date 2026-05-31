const styles = {
  Open: 'bg-sfm-green-950 text-sfm-green-400 border-sfm-green-800',
  InProgress: 'bg-amber-950/50 text-amber-400 border-amber-800/50',
  Closed: 'bg-gray-800 text-gray-400 border-gray-700',
  Pending: 'bg-sfm-green-950 text-sfm-green-300 border-sfm-green-800',
  Accepted: 'bg-sfm-green-900/80 text-sfm-green-300 border-sfm-green-700',
  Rejected: 'bg-red-950/50 text-red-400 border-red-800/50',
  Working: 'bg-amber-950/50 text-amber-400 border-amber-800/50',
  Completed: 'bg-sfm-green-950 text-sfm-green-400 border-sfm-green-800',
  default: 'bg-sfm-green-950 text-sfm-green-400 border-sfm-green-800',
};

export default function Badge({ children, status }) {
  const key = status?.replace(/\s/g, '') || 'default';
  const style = styles[key] || styles.default;
  return (
    <span className={`inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold uppercase tracking-wide ${style}`}>
      {children || status}
    </span>
  );
}
