export default function Card({ children, className = '', hover = false, ...props }) {
  return (
    <div
      className={`rounded-2xl border border-sfm-border/80 bg-sfm-card/80 backdrop-blur-sm p-6 shadow-xl shadow-black/40 ${
        hover ? 'transition-all duration-300 hover:border-sfm-green-800/60 hover:shadow-sfm-green-900/20 hover:-translate-y-0.5' : ''
      } ${className}`}
      {...props}
    >
      {children}
    </div>
  );
}
