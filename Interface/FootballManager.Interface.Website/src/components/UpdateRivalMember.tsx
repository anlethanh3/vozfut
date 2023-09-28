import { Button, Modal, FloatingLabel, Form } from 'react-bootstrap';
import { useState } from 'react';
import { UpdateRivalMemberProps } from '../slices/matchDetailSlice';
import { MemberProps } from '../slices/memberSlice';
import { MatchProps } from '../slices/matchSlice';

export default function UpdateRivalMember(props: { show: boolean, match: MatchProps | undefined, members: MemberProps[], onSubmit: (data: UpdateRivalMemberProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose, members, match } = props
    const [member, setMember] = useState<UpdateRivalMemberProps>({ matchId: 0, isGK: false, isIn: true, memberId: -1, teamId: -1 })
    const onValid = () => {
        if (member.memberId === -1 || member.teamId === -1) {
            return
        }
        onSubmit(member)
    }
    let infos = teamColors(match?.teamCount ?? 3)
    function teamColors(teamCount: number) {
        if (teamCount < 3) {
            return [
                { color: 'secondary', name: 'Orange' },
                { color: 'warning', name: 'Banana' },
            ]
        }
        if (teamCount < 4) {
            return [
                { color: 'secondary', name: 'Orange' },
                { color: 'primary', name: 'Blue' },
                { color: 'warning', name: 'Banana' },
            ]
        }
        return [
            { color: 'danger', name: 'Red' },
            { color: 'primary', name: 'Blue' },
            { color: 'warning', name: 'Banana' },
            { color: 'secondary', name: 'Orange' },
        ]
    }

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Update Rival Member In Out</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="updateRivalMember.TeamId">
                        <FloatingLabel controlId="floatingTeamId" label="Team">
                            <Form.Select value={member.teamId} onChange={(e) => setMember({ ...member, teamId: parseInt(e.target.value) })}>
                                <option hidden value={-1}>Select a Team</option>
                                {
                                    infos.map((value, index) =>
                                        <option key={`TeamId.id-${index}`} value={index}>{value.name}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="updateRivalMember.MemberId">
                        <FloatingLabel controlId="floatingMemberId" label="Member In">
                            <Form.Select value={member.memberId} onChange={(e) => setMember({ ...member, memberId: parseInt(e.target.value) })}>
                                <option hidden value={0}>Select a Member</option>
                                {
                                    members.map(value =>
                                        <option key={`MemberId.id-${value.id}`} value={value.id}>{value.name}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="updateRivalMember.MemberGK">
                        <Form.Check label="Member Is GK" type='switch' checked={member.isGK}
                            onChange={(e) => setMember({ ...member, isGK: e.target.checked })} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="updateRivalMember.MemberInOut">
                        <Form.Check label="Member Will In" type='switch' checked={member.isIn}
                            onChange={(e) => setMember({ ...member, isIn: e.target.checked })} />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Confirm
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}