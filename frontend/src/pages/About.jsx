import Card from '../components/ui/Card';
import PageHeader from '../components/ui/PageHeader';

export default function About() {
  return (
    <div className="mx-auto max-w-3xl">
      <PageHeader title="About SFM" subtitle="Somali Freelance Marketplace" />
      <Card className="mb-6">
        <p className="leading-relaxed text-gray-300">
          Somali Freelance Marketplace (SFM) waa platform loogu talagalay in freelancers iyo
          clients-ka Soomaaliyeed ay si fudud u wada shaqeeyaan online.
        </p>
      </Card>
      <Card>
        <h3 className="mb-4 text-lg font-semibold text-sfm-green-400">Ujeedada</h3>
        <ul className="space-y-3 text-gray-400">
          {['Shaqo online lagu kala iibsado', 'Freelancers helaan jobs', 'Clients helaan workers la aamini karo', 'Payments si ammaan ah (escrow)'].map((item) => (
            <li key={item} className="flex items-center gap-3">
              <span className="flex h-6 w-6 shrink-0 items-center justify-center rounded-full bg-sfm-green-950 text-xs text-sfm-green-400">✓</span>
              {item}
            </li>
          ))}
        </ul>
      </Card>
    </div>
  );
}
