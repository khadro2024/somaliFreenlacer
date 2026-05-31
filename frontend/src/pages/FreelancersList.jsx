import { useEffect, useState } from 'react';
import { profilesApi } from '../api/client';
import Card from '../components/ui/Card';
import PageHeader from '../components/ui/PageHeader';
import { Avatar } from '../components/ui/ImageUpload';

export default function FreelancersList() {
  const [freelancers, setFreelancers] = useState([]);

  useEffect(() => {
    profilesApi.getFreelancers().then((r) => setFreelancers(r.data)).catch(() => {});
  }, []);

  return (
    <div className="mx-auto max-w-7xl">
      <PageHeader title="Freelancers" subtitle="Shaqaale xirfad leh oo la aamini karo" />
      <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
        {freelancers.map((f) => (
          <Card key={f.profileId} hover>
            <div className="flex items-start gap-4">
              <Avatar src={f.profileImageUrl} name={f.fullName} size="md" />
              <div>
                <h3 className="font-semibold text-white">{f.fullName}</h3>
                <p className="mt-1 text-sm text-sfm-green-400">★ {f.rating.toFixed(1)} ({f.ratingCount} reviews)</p>
              </div>
            </div>
            <p className="mt-4 text-sm text-gray-400 line-clamp-2">{f.bio || 'No bio yet.'}</p>
            <p className="mt-3 text-xs text-gray-500"><span className="text-gray-400">Skills:</span> {f.skills || '—'}</p>
            <p className="mt-2 text-sm font-semibold text-sfm-green-400">${f.hourlyRate}/hr</p>
            {f.portfolioImages?.length > 0 && (
              <div className="mt-4 flex gap-2 overflow-x-auto">
                {f.portfolioImages.slice(0, 3).map((img) => (
                  <img key={img.workImageId} src={img.imageUrl} alt="" className="h-16 w-20 shrink-0 rounded-lg object-cover" />
                ))}
              </div>
            )}
          </Card>
        ))}
      </div>
    </div>
  );
}
