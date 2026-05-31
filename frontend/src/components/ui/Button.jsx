const variants = {
  primary:
    'bg-gradient-to-r from-sfm-green-800 to-sfm-green-900 text-white border border-sfm-green-600/40 shadow-lg shadow-sfm-green-900/30 hover:from-sfm-green-700 hover:to-sfm-green-800 hover:shadow-sfm-green-500/20',
  outline:
    'bg-transparent text-sfm-green-400 border border-sfm-green-800 hover:bg-sfm-green-950 hover:border-sfm-green-600',
  danger: 'bg-red-600/90 text-white border border-red-500/50 hover:bg-red-500',
  ghost: 'bg-transparent text-gray-400 hover:text-sfm-green-400 hover:bg-white/5',
};

const sizes = {
  sm: 'px-3 py-1.5 text-sm',
  md: 'px-5 py-2.5 text-sm',
  lg: 'px-6 py-3 text-base',
};

export default function Button({
  children,
  variant = 'primary',
  size = 'md',
  className = '',
  ...props
}) {
  return (
    <button
      type="button"
      className={`inline-flex items-center justify-center gap-2 rounded-xl font-semibold transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed ${variants[variant]} ${sizes[size]} ${className}`}
      {...props}
    >
      {children}
    </button>
  );
}
