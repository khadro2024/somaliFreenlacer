export function Label({ children, className = '' }) {
  return <label className={`mb-1.5 block text-sm font-medium text-gray-400 ${className}`}>{children}</label>;
}

export function Input({ className = '', ...props }) {
  return (
    <input
      className={`w-full rounded-xl border border-sfm-border bg-sfm-surface px-4 py-2.5 text-gray-100 placeholder:text-gray-600 transition focus:border-sfm-green-600 focus:outline-none focus:ring-2 focus:ring-sfm-green-600/20 ${className}`}
      {...props}
    />
  );
}

export function Textarea({ className = '', ...props }) {
  return (
    <textarea
      className={`w-full rounded-xl border border-sfm-border bg-sfm-surface px-4 py-2.5 text-gray-100 placeholder:text-gray-600 transition focus:border-sfm-green-600 focus:outline-none focus:ring-2 focus:ring-sfm-green-600/20 ${className}`}
      {...props}
    />
  );
}

export function Select({ className = '', children, ...props }) {
  return (
    <select
      className={`w-full rounded-xl border border-sfm-border bg-sfm-surface px-4 py-2.5 text-gray-100 transition focus:border-sfm-green-600 focus:outline-none focus:ring-2 focus:ring-sfm-green-600/20 ${className}`}
      {...props}
    >
      {children}
    </select>
  );
}

export function FormGroup({ label, children }) {
  return (
    <div className="mb-4">
      {label && <Label>{label}</Label>}
      {children}
    </div>
  );
}
