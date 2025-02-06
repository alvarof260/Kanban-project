import { ProfileIcon, ProfileInfo } from "../";

export const ProfileSummary = () => {
  return (
    <section className='w-full flex flex-row items-center gap-2'>
      <ProfileIcon />
      <ProfileInfo />
    </section>
  );
};
