export default function PageHeader({ title, subtitle, action }) {
  return (
    <div className="mb-8 flex flex-col gap-4 sm:flex-row sm:items-end sm:justify-between">
      <div>
        <h1 className="text-3xl font-bold tracking-tight text-white md:text-4xl">
          <span className="bg-gradient-to-r from-sfm-green-300 to-sfm-green-500 bg-clip-text text-transparent">
            {title}
          </span>
        </h1>
        {subtitle && <p className="mt-2 text-gray-400">{subtitle}</p>}
      </div>
      {action}
    </div>
  );
}
