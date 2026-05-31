import { Link } from 'react-router-dom';
import Button from '../components/ui/Button';
import Card from '../components/ui/Card';

const features = [
  { title: 'Escrow Payments', desc: 'Lacagta waa la hayaa ilaa shaqada la dhammeeyo — aamin & nabadgelyo.', icon: '🔒' },
  { title: 'Verified Workers', desc: 'Freelancers la xaqiijiyay oo leh rating iyo reviews.', icon: '✓' },
  { title: 'Job Matching', desc: 'Clients waxay helaan shaqaale xirfad leh si fudud.', icon: '⚡' },
  { title: 'Messaging', desc: 'Isgaarsiin toos ah inta mashruuca socdo.', icon: '💬' },
];

export default function Home() {
  return (
    <div className="mx-auto max-w-7xl">
      <section className="relative overflow-hidden rounded-3xl border border-sfm-green-900/30 bg-gradient-to-br from-sfm-card via-sfm-black to-sfm-green-950/20 px-6 py-16 sm:px-12 sm:py-24">
        <div className="pointer-events-none absolute -left-20 top-0 h-72 w-72 rounded-full bg-sfm-green-600/10 blur-3xl" />
        <div className="pointer-events-none absolute -right-10 bottom-0 h-64 w-64 rounded-full bg-sfm-green-500/5 blur-3xl" />

        <div className="relative grid gap-12 lg:grid-cols-2 lg:items-center">
          <div>
            <p className="mb-4 inline-flex items-center gap-2 rounded-full border border-sfm-green-800/50 bg-sfm-green-950/50 px-4 py-1.5 text-xs font-semibold uppercase tracking-wider text-sfm-green-400">
              <span className="h-1.5 w-1.5 animate-pulse rounded-full bg-sfm-green-500" />
              Marketplace-ka Soomaaliyeed
            </p>
            <h1 className="text-4xl font-extrabold leading-tight tracking-tight text-white sm:text-5xl lg:text-6xl">
              Shaqo online ah,{' '}
              <span className="bg-gradient-to-r from-sfm-green-300 via-sfm-green-400 to-emerald-500 bg-clip-text text-transparent">
                kalsooni buuxda
              </span>
            </h1>
            <p className="mt-6 max-w-lg text-lg text-gray-400">
              Somali Freelance Marketplace — platform isku xira freelancers iyo clients
              iyadoo lacagtu escrow ku ammaan tahay.
            </p>
            <div className="mt-10 flex flex-wrap gap-4">
              <Link to="/register"><Button size="lg">Bilow Hadda →</Button></Link>
              <Link to="/jobs"><Button variant="outline" size="lg">Baadh Shaqooyinka</Button></Link>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-4 sm:grid-cols-3 lg:grid-cols-1 xl:grid-cols-3">
            {[
              { n: '500+', l: 'Freelancers' },
              { n: '200+', l: 'Jobs Posted' },
              { n: '98%', l: 'Success Rate' },
            ].map((s) => (
              <div
                key={s.l}
                className="rounded-2xl border border-sfm-green-800/30 bg-sfm-black/40 p-6 text-center backdrop-blur"
              >
                <p className="text-3xl font-bold text-sfm-green-400">{s.n}</p>
                <p className="mt-1 text-sm text-gray-500">{s.l}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      <section className="mt-20">
        <h2 className="mb-10 text-center text-2xl font-bold text-white sm:text-3xl">
          Maxaa ka dhigaya <span className="text-sfm-green-400">SFM</span>?
        </h2>
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
          {features.map((f) => (
            <Card key={f.title} hover>
              <span className="text-3xl">{f.icon}</span>
              <h3 className="mt-4 text-lg font-semibold text-sfm-green-300">{f.title}</h3>
              <p className="mt-2 text-sm leading-relaxed text-gray-400">{f.desc}</p>
            </Card>
          ))}
        </div>
      </section>
    </div>
  );
}
