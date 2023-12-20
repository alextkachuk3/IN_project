export class MusicInfoDto {
  constructor(
    public id: string,
    public name: string,
    public uploaderName: string,
    public isLiked: boolean
  ) { }
}
