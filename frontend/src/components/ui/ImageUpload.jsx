import { useEffect, useRef, useState } from 'react';
import { mediaApi } from '../../api/client';
import Button from './Button';
import Alert from './Alert';

export default function ImageUpload({
  type,
  jobId,
  projectId,
  label = 'Upload image',
  onUploaded,
  className = '',
}) {
  const inputRef = useRef(null);
  const [preview, setPreview] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [configured, setConfigured] = useState(true);

  useEffect(() => {
    mediaApi.getConfig().then((r) => setConfigured(r.data.cloudinaryConfigured)).catch(() => {});
  }, []);

  const handleFile = async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setPreview(URL.createObjectURL(file));
    setError('');
    setLoading(true);
    const form = new FormData();
    form.append('file', file);
    form.append('type', type);
    if (jobId) form.append('jobId', String(jobId));
    if (projectId) form.append('projectId', String(projectId));
    try {
      const { data } = await mediaApi.upload(form);
      onUploaded?.(data);
    } catch (err) {
      setError(err.response?.data?.message || 'Upload failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={`rounded-xl border border-dashed border-sfm-green-800/50 bg-sfm-surface/30 p-4 ${className}`}>
      <p className="mb-2 text-sm font-medium text-gray-400">{label}</p>
      {!configured && (
        <Alert>Cloudinary ma configured. Ku dar credentials backend/appsettings.json</Alert>
      )}
      {error && <Alert>{error}</Alert>}
      {preview && (
        <img src={preview} alt="Preview" className="mb-3 h-32 w-full rounded-lg object-cover" />
      )}
      <input ref={inputRef} type="file" accept="image/*" className="hidden" onChange={handleFile} />
      <Button type="button" variant="outline" size="sm" disabled={loading} onClick={() => inputRef.current?.click()}>
        {loading ? 'Uploading...' : 'Choose image'}
      </Button>
      <p className="mt-2 text-xs text-gray-600">JPG, PNG, WEBP — max 5MB</p>
    </div>
  );
}

export function Avatar({ src, name, size = 'lg' }) {
  const sizes = { sm: 'h-10 w-10 text-sm', md: 'h-14 w-14 text-lg', lg: 'h-24 w-24 text-2xl' };
  if (src) {
    return <img src={src} alt={name} className={`${sizes[size]} rounded-2xl object-cover ring-2 ring-sfm-green-800/50`} />;
  }
  return (
    <div className={`${sizes[size]} flex items-center justify-center rounded-2xl bg-gradient-to-br from-sfm-green-800 to-sfm-green-950 font-bold text-sfm-green-300 ring-2 ring-sfm-green-800/50`}>
      {name?.charAt(0) || '?'}
    </div>
  );
}

export function ImageGallery({ images, onDelete }) {
  if (!images?.length) return null;
  return (
    <div className="mt-4 grid grid-cols-2 gap-3 sm:grid-cols-3">
      {images.map((img) => (
        <figure key={img.workImageId || img.imageUrl} className="group relative overflow-hidden rounded-xl border border-sfm-border">
          <img src={img.imageUrl} alt={img.caption} className="h-32 w-full object-cover" />
          {img.caption && <figcaption className="p-2 text-xs text-gray-500">{img.caption}</figcaption>}
          {onDelete && img.workImageId && (
            <button
              type="button"
              onClick={() => onDelete(img.workImageId)}
              className="absolute right-2 top-2 rounded bg-black/70 px-2 py-1 text-xs text-red-300 opacity-0 transition group-hover:opacity-100"
            >
              Delete
            </button>
          )}
        </figure>
      ))}
    </div>
  );
}
