import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { useEffect, useState } from 'react';
import { MemberProps } from '../reducers/MemberReducer';

const UpdateMember = (props: { show: boolean, initData: MemberProps | undefined, onSubmit: (member: MemberProps) => void, onClose: () => void }) => {
    const { show, initData, onSubmit, onClose } = props
    const stats = [1, 2, 3, 4, 5]
    const [member, setMember] = useState<MemberProps>({ id: 0, name: '', description: '', elo: 1, })
    const [valid, setValid] = useState({ name: true });
    const onValid = () => {
        if (member.name === '') {
            setValid({ ...valid, name: false })
            return
        }
        onSubmit(member)
    }
    useEffect(() => {
        if (initData) {
            setMember({ ...initData })
        }
    }, [initData])

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Update a member</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="addMember.realName">
                        <FloatingLabel controlId="floatingrealName" label="realName">
                            <Form.Control type="text" required placeholder="Input realName" value={member.realName} onChange={(e) => setMember({ ...member, realName: e.target.value })} />
                            <Form.Control.Feedback type="invalid">realName is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.Name">
                        <FloatingLabel controlId="floatingName" label="Name">
                            <Form.Control type="text" required isInvalid={!valid.name} placeholder="Input name" value={member.name} onChange={(e) => setMember({ ...member, name: e.target.value })} />
                            <Form.Control.Feedback type="invalid">Name is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.Description">
                        <FloatingLabel controlId="floatingDescription" label="Description">
                            <Form.Control type="text" placeholder="Input description" value={member.description} onChange={(e) => setMember({ ...member, description: e.target.value })} />
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.Speed">
                        <FloatingLabel controlId="floatingSpeed" label="Speed">
                            <Form.Select value={member.speed} onChange={(e) => setMember({ ...member, speed: parseInt(e.target.value) })}>
                                {
                                    stats.map(value =>
                                        <option key={`speed.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.Stamina">
                        <FloatingLabel controlId="floatingstamina" label="stamina">
                            <Form.Select value={member.stamina} onChange={(e) => setMember({ ...member, stamina: parseInt(e.target.value) })}>
                                {
                                    stats.map(value =>
                                        <option key={`stamina.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.finishing">
                        <FloatingLabel controlId="floatingfinishing" label="finishing">
                            <Form.Select value={member.finishing} onChange={(e) => setMember({ ...member, finishing: parseInt(e.target.value) })}>
                                {
                                    stats.map(value =>
                                        <option key={`finishing.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.passing">
                        <FloatingLabel controlId="floatingpassing" label="passing">
                            <Form.Select value={member.passing} onChange={(e) => setMember({ ...member, passing: parseInt(e.target.value) })}>
                                {
                                    stats.map(value =>
                                        <option key={`passing.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.skill">
                        <FloatingLabel controlId="floatingskill" label="skill">
                            <Form.Select value={member.skill} onChange={(e) => setMember({ ...member, skill: parseInt(e.target.value) })}>
                                {
                                    stats.map(value =>
                                        <option key={`skill.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Update
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}

export default UpdateMember