export default function Alert({ children, variant = 'error' }) {
  const styles = {
    error: 'border-red-500/50 bg-red-950/30 text-red-200',
    success: 'border-sfm-green-600/50 bg-sfm-green-950/40 text-sfm-green-300',
  };
  return (
    <div className={`mb-4 rounded-xl border px-4 py-3 text-sm ${styles[variant]}`}>{children}</div>
  );
}
