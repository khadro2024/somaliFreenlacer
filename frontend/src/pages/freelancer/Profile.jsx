import { useEffect, useState } from 'react';
import { profilesApi, mediaApi } from '../../api/client';
import { useAuth } from '../../context/AuthContext';
import Card from '../../components/ui/Card';
import Button from '../../components/ui/Button';
import Alert from '../../components/ui/Alert';
import PageHeader from '../../components/ui/PageHeader';
import { FormGroup, Input, Textarea } from '../../components/ui/Input';
import ImageUpload, { Avatar, ImageGallery } from '../../components/ui/ImageUpload';

export default function Profile() {
  const { user } = useAuth();
  const [form, setForm] = useState({ skills: '', bio: '', hourlyRate: '', portfolio: '' });
  const [profileImageUrl, setProfileImageUrl] = useState('');
  const [portfolioImages, setPortfolioImages] = useState([]);
  const [msg, setMsg] = useState('');

  const load = () => {
    if (!user) return;
    profilesApi.getProfile(user.userId).then((r) => {
      const p = r.data;
      setForm({ skills: p.skills, bio: p.bio, hourlyRate: String(p.hourlyRate), portfolio: p.portfolio });
      setProfileImageUrl(p.profileImageUrl || '');
      setPortfolioImages(p.portfolioImages || []);
    }).catch(() => {});
  };

  useEffect(() => { load(); }, [user]);

  const save = async (e) => {
    e.preventDefault();
    await profilesApi.updateMe({
      skills: form.skills,
      bio: form.bio,
      hourlyRate: parseFloat(form.hourlyRate) || 0,
      portfolio: form.portfolio,
    });
    setMsg('Profile saved!');
  };

  const deletePortfolio = async (id) => {
    await mediaApi.delete(id);
    load();
  };

  return (
    <div>
      <PageHeader title="My Profile" />
      {msg && <Alert variant="success">{msg}</Alert>}

      <div className="mb-8 flex flex-wrap items-center gap-6">
        <Avatar src={profileImageUrl} name={user?.fullName} size="lg" />
        <ImageUpload type="profile" label="Sawirka profile-ka" onUploaded={(d) => { setProfileImageUrl(d.imageUrl); setMsg(d.message); }} />
      </div>

      <Card className="mb-8 max-w-xl">
        <form onSubmit={save}>
          <FormGroup label="Skills"><Input value={form.skills} onChange={(e) => setForm({ ...form, skills: e.target.value })} /></FormGroup>
          <FormGroup label="Bio"><Textarea rows={4} value={form.bio} onChange={(e) => setForm({ ...form, bio: e.target.value })} /></FormGroup>
          <FormGroup label="Hourly Rate ($)"><Input type="number" value={form.hourlyRate} onChange={(e) => setForm({ ...form, hourlyRate: e.target.value })} /></FormGroup>
          <FormGroup label="Portfolio link"><Input value={form.portfolio} onChange={(e) => setForm({ ...form, portfolio: e.target.value })} /></FormGroup>
          <Button type="submit">Save Profile</Button>
        </form>
      </Card>

      <Card>
        <h3 className="mb-2 text-lg font-semibold text-sfm-green-400">Portfolio — Shaqooyin aad qabatay</h3>
        <p className="mb-4 text-sm text-gray-500">Soo geli sawirrada shaqadaada (Cloudinary)</p>
        <ImageUpload type="portfolio" label="Ku dar sawir portfolio" onUploaded={() => load()} />
        <ImageGallery images={portfolioImages} onDelete={deletePortfolio} />
      </Card>
    </div>
  );
}
